using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using MvcPaging;

namespace MvcPaging
{
	public static class PagingExtensions
	{
		#region HtmlHelper extensions

    public static MvcHtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, string routeName = "")
		{
			return Pager(htmlHelper, pageSize, currentPage, totalItemCount, null, null, routeName);
		}

    public static MvcHtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, string actionName, string routeName = "")
		{
			return Pager(htmlHelper, pageSize, currentPage, totalItemCount, actionName, null, routeName);
		}

    public static MvcHtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, object values, string routeName = "")
		{
			return Pager(htmlHelper, pageSize, currentPage, totalItemCount, null, new RouteValueDictionary(values), routeName);
		}

    public static MvcHtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, string actionName, object values, string routeName = "")
		{
			return Pager(htmlHelper, pageSize, currentPage, totalItemCount, actionName, new RouteValueDictionary(values),routeName);
		}

    public static MvcHtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, RouteValueDictionary valuesDictionary, string routeName = "")
		{
			return Pager(htmlHelper, pageSize, currentPage, totalItemCount, null, valuesDictionary, routeName);
		}

		public static MvcHtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, string actionName, RouteValueDictionary valuesDictionary, string routeName="")
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
			var pager = new Pager(htmlHelper.ViewContext, pageSize, currentPage, totalItemCount, valuesDictionary, routeName);
			return pager.RenderHtml();
		}

		#endregion

		#region IQueryable<T> extensions

		public static IPagedList<T> ToPagedList<T>(this IQueryable<T> source, int pageIndex, int pageSize)
		{
			return new PagedList<T>(source, pageIndex, pageSize);
		}

		public static IPagedList<T> ToPagedList<T>(this IQueryable<T> source, int pageIndex, int pageSize, int totalCount)
		{
			return new PagedList<T>(source, pageIndex, pageSize, totalCount);
		}

		#endregion

		#region IEnumerable<T> extensions

		public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
		{
			return new PagedList<T>(source, pageIndex, pageSize);
		}

		public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
		{
			return new PagedList<T>(source, pageIndex, pageSize, totalCount);
		}

		#endregion
	}
}