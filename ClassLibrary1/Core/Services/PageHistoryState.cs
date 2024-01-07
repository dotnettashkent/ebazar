using Microsoft.AspNetCore.Components;
using Shared.Infrastructures;
using Stl.Fusion;

namespace Client.Core.Services
{
	public class PageHistoryState
	{
		private List<string> previousPages;
		public NavigationManager NavManager { get; }

		public PageHistoryState(NavigationManager navManager)
		{
			previousPages = new List<string>();
			NavManager = navManager;
		}
		public void AddPageToHistory(string pageName)
		{
			previousPages.Add(pageName);
		}

		public string GetGoBackPage()
		{
			if (previousPages.Count > 1)
			{
				// You add a page on initialization, so you need to return the 2nd from the last
				return previousPages.ElementAt(previousPages.Count - 1);
			}

			// Can't go back because you didn't navigate enough
			return previousPages.First();
		}

		public bool CanGoBack()
		{
			return previousPages.Count > 1;
		}

		public void Back(string route)
		{
			if (CanGoBack())
			{
				string prevPage = GetGoBackPage();
				NavManager.NavigateTo(prevPage);
			}
			else
			{
				NavManager.NavigateTo($"/{route}");
			}
		}

		public void SetPage(IMutableState<TableOptions> MutableState, bool isOnParametrSet = false)
		{
			var queryParams = new Dictionary<string, object?>
			{
				["page"] = MutableState.Value.Page.ToString(),
				["search"] = string.Empty == MutableState.Value.Search ? null : MutableState.Value.Search,
			};

			var newUri = NavManager.GetUriWithQueryParameters(queryParams);
			AddPageToHistory(newUri);

			NavManager.NavigateTo(newUri);

		}
		public void SetPage(string Role, bool isOnParametrSet = false)
		{
			var queryParams = new Dictionary<string, object?>
			{
				["role"] = string.Empty == Role ? null : Role,
			};

			var newUri = NavManager.GetUriWithQueryParameters(queryParams);
			AddPageToHistory(newUri);

			NavManager.NavigateTo(newUri);

		}
	}
}
