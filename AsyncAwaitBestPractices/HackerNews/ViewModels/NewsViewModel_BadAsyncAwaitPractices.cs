using System.Collections.Frozen;
using System.Diagnostics;
using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HackerNews;

partial class NewsViewModel_BadAsyncAwaitPractices : BaseViewModel
{
	readonly HackerNewsAPIService _hackerNewsAPIService;
	readonly AsyncAwaitBestPractices.WeakEventManager _pullToRefreshEventManager = new();

	public NewsViewModel_BadAsyncAwaitPractices(IDispatcher dispatcher, HackerNewsAPIService hackerNewsAPIService) : base(dispatcher)
	{
		_hackerNewsAPIService = hackerNewsAPIService;

        //ToDo Refactor
        //Refresh(CancellationToken.None);

        //try
        //{
        //	Refresh();
        //}
        //catch (Exception ex)
        //{ 

        //}

        ////Extra codes (for race condition)
        //TopStoryCollection.Clear();
        //TopStoryCollection.Add();

        Refresh(CancellationToken.None).SafeFireAndForget<HttpRequestException>(onException: ex => Trace.WriteLine(ex.HttpRequestError));
    }

    //async void Refresh()
    //{
    //	await Refresh(CancellationToken.None);
    //	throw new Exception();
    //}

    public event EventHandler<string> PullToRefreshFailed
	{
		add => _pullToRefreshEventManager.AddEventHandler(value);
		remove => _pullToRefreshEventManager.RemoveEventHandler(value);
	}


	[ObservableProperty]
	public partial bool IsListRefreshing { get; set; }

	[RelayCommand]
	async Task Refresh(CancellationToken token)
	{
        // ToDo Refactor
        //var minimumRefreshTimeTask = Task.Delay(TimeSpan.FromSeconds(2));
        var minimumRefreshTimeTask = Task.Delay(TimeSpan.FromSeconds(2), token);
		//var minimumRefreshTimeTask = Task.Delay(TimeSpan.FromSeconds(2)).WaitAsync(token);

        try
        {
            // ToDo Refactor
            //var topStoriesList = await GetTopStories(token, StoriesConstants.NumberOfStories);
            //var topStoriesList = await GetTopStories(token, StoriesConstants.NumberOfStories).ConfigureAwait(false);

            TopStoryCollection.Clear();
            await foreach (var story in GetTopStories(token, StoriesConstants.NumberOfStories).ConfigureAwait(false))
			{
                if (!TopStoryCollection.Any(x => x.Title.Equals(story.Title, StringComparison.Ordinal)))
                    InsertIntoSortedCollection(TopStoryCollection, (a, b) => b.Score.CompareTo(a.Score), story);
            }


			//TopStoryCollection.Clear();
			//foreach (var story in topStoriesList)
			//{
			//	if (!TopStoryCollection.Any(x => x.Title.Equals(story.Title, StringComparison.Ordinal)))
			//		InsertIntoSortedCollection(TopStoryCollection, (a, b) => b.Score.CompareTo(a.Score), story);
			//}
		}
		catch (Exception e)
		{
			OnPullToRefreshFailed(e.ToString());
		}
		finally
		{
			// ToDo Refactor
			//minimumRefreshTimeTask.Wait();
			await minimumRefreshTimeTask.ConfigureAwait(false);

			IsListRefreshing = false;
		}
	}

    // ToDo Refactor
    //async Task<FrozenSet<StoryModel>> GetTopStories(CancellationToken token, int storyCount = int.MaxValue)
    async IAsyncEnumerable<StoryModel> GetTopStories([IEnumeratorCancellation] CancellationToken token, int storyCount = int.MaxValue)

    {
		var topStoryIds = await GetTopStoryIDs(token).ConfigureAwait(false);

		//foreach (var topStoryId in topStoryIds)
		//{
		//	var story = await GetStory(topStoryId, token).ConfigureAwait(false);
		//	topStoryList.Add(story);

		//	if (topStoryList.Count >= storyCount)
		//		break;
		//}

		//return topStoryList.OrderByDescending(x => x.Score).ToFrozenSet();

		List<Task<StoryModel>> getTopStoriesTasks = topStoryIds.Select(id => GetStory(id, token)).ToList();
		
		//var topStories = await Task.WhenAll(getTopStoriesTasks).ConfigureAwait(false);
		//return topStories.ToFrozenSet();
	
		await foreach(var topStoryTask in Task.WhenEach(getTopStoriesTasks).WithCancellation(token))
		{
			if (storyCount == 0)
				break;

			//yield return topStoryTask.Result; //Task is complete fro sure because of WhenEach so we can use .Result but still we avoid that because of refactoring and code changings in future. 

			var topStory = await topStoryTask.ConfigreAwait(false);
			yield return topStory;

			storyCount--;
		}
	
	}

	//ToDo Refactor

	//async Task<StoryModel> GetStory(long storyId, CancellationToken token)
	//{
	//	return await _hackerNewsAPIService.GetStory(storyId, token).ConfigureAwait(false);
	//}
	Task<StoryModel> GetStory(long storyId, CancellationToken token)
    {
        return _hackerNewsAPIService.GetStory(storyId, token);
    }

    //ToDo Refactor
    //async Task<FrozenSet<long>> GetTopStoryIDs(CancellationToken token)
    async ValueTask<FrozenSet<long>> GetTopStoryIDs(CancellationToken token)
	{
		if (IsDataRecent(TimeSpan.FromHours(1)))
			return TopStoryCollection.Select(x => x.Id).ToFrozenSet();

		try
		{
			return await _hackerNewsAPIService.GetTopStoryIDs(token).ConfigureAwait(false);
		}
		catch (Exception e)
		{
			Trace.WriteLine(e.Message);
			throw;
		}
	}

	bool IsDataRecent(TimeSpan timeSpan) => (DateTimeOffset.UtcNow - TopStoryCollection.Max(x => x.CreatedAt_DateTimeOffset)) > timeSpan;

	void OnPullToRefreshFailed(string message) => _pullToRefreshEventManager.RaiseEvent(this, message, nameof(PullToRefreshFailed));
}