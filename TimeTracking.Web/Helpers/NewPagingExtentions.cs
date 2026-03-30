using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcPaging
{
	public static class NewPagingExtensions
	{
		

		public static MvcHtmlString NewPager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount)
		{
			return NewPager(htmlHelper, pageSize, currentPage, totalItemCount, null, null);
		}

		public static MvcHtmlString NewPager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, string actionName)
		{
			return NewPager(htmlHelper, pageSize, currentPage, totalItemCount, actionName, null);
		}

		public static MvcHtmlString NewPager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, object values)
		{
			return NewPager(htmlHelper, pageSize, currentPage, totalItemCount, null, new RouteValueDictionary(values));
		}

		public static MvcHtmlString NewPager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, string actionName, object values)
		{
			return NewPager(htmlHelper, pageSize, currentPage, totalItemCount, actionName, new RouteValueDictionary(values));
		}

		public static MvcHtmlString NewPager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, RouteValueDictionary valuesDictionary)
		{
			return NewPager(htmlHelper, pageSize, currentPage, totalItemCount, null, valuesDictionary);
		}

		public static MvcHtmlString NewPager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, string actionName, RouteValueDictionary valuesDictionary)
		{
			if (valuesDictionary == null)
			{
				valuesDictionary = new RouteValueDictionary();
			}
			if (actionName != null)
			{
				if (valuesDictionary.ContainsKey("action"))
				{
					throw new ArgumentException("The valuesDictionary already contains an action.", "actionName");
				}
				valuesDictionary.Add("action", actionName);
			}
			var pager = new NewPager(htmlHelper.ViewContext, pageSize, currentPage, totalItemCount, valuesDictionary);
			return pager.RenderHtml();
		}

		public static MvcHtmlString NewPagerScript(this HtmlHelper htmlHelper, string htmlTableID, string dataSetName, string fetchDataUrl, string btnExportContainerID, string pageNumberGlobalContainerID)
		{
			var sb = new StringBuilder();

			sb.AppendLine($@"$(document).on('click', '#{htmlTableID} .page', function(event) {{
								event.preventDefault();
								if (!$(this).hasClass('disabled') && !$(this).hasClass('current')) {{
									const nextPage = $(this).data('page');");
			if (!string.IsNullOrWhiteSpace(pageNumberGlobalContainerID))
			{
				sb.AppendLine($@"if($(document).find('#{pageNumberGlobalContainerID}').length > 0) {{
                                ('#{pageNumberGlobalContainerID}').val(nextPage);
				}}");

			}

			sb.AppendLine($@"fetch{dataSetName}(nextPage);
								}}
							}});
						");

			sb.AppendLine($@"function fetch{dataSetName}(page) {{
    

								const urlWithParams = `{fetchDataUrl}&page=${{page}}`;

								$.get(urlWithParams, (response, b) => {{
									$('#{htmlTableID}').replaceWith(response);
						");
			if (!string.IsNullOrWhiteSpace(btnExportContainerID))
			{
				sb.AppendLine($@"if ($('#{htmlTableID}').find('.no-results').length > 0) {{
										$('#{btnExportContainerID}').css('display', 'none');
									}}
									else {{
										$('#{btnExportContainerID}').css('display', 'flex');
									}} ");
			}

			sb.AppendLine($@"	}});
							}}");

			return MvcHtmlString.Create(sb.ToString());
		}
	}
}