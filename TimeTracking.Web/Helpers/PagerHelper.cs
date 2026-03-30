using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcPaging
{
	public class NewPager
	{
		private ViewContext viewContext;
		private readonly int pageSize;
		private readonly int currentPage;
		private readonly int totalItemCount;
		private readonly RouteValueDictionary linkWithoutPageValuesDictionary;

		const int INITIAL_PAGE_LIMIT = 5;
		const int VISIBLE_PAGES = 3;

		public NewPager(ViewContext viewContext, int pageSize, int currentPage, int totalItemCount, RouteValueDictionary valuesDictionary)
		{
			this.viewContext = viewContext;
			this.pageSize = pageSize;
			this.currentPage = currentPage;
			this.totalItemCount = totalItemCount;
			this.linkWithoutPageValuesDictionary = valuesDictionary;
			//this.routeName = routeName;
		}

		public MvcHtmlString RenderHtml()
		{
			int pageCount = (int)Math.Ceiling(this.totalItemCount / (double)this.pageSize);

			var sb = new StringBuilder();

			// Previous
			if (this.currentPage > 1)
			{
				sb.Append(GeneratePageLink("<div><i class=\"fa fa-long-arrow-left\"></i>&nbsp;<div>Previous</div></div>", this.currentPage - 1));
			}
			else
			{
				sb.Append("<div class=\"page disabled\"><div><i class=\"fa fa-long-arrow-left\"></i>&nbsp;<div>Previous</div></div></div>");
			}

			int start = 1;
			int end = pageCount;

			if (pageCount >= INITIAL_PAGE_LIMIT + VISIBLE_PAGES)
			{
				if (currentPage < INITIAL_PAGE_LIMIT)
				{
					for (int i = start; i <= INITIAL_PAGE_LIMIT; i++)
					{
						if (i == this.currentPage)
						{
							sb.AppendFormat("<div class=\"page current\">{0}</div>", i);
						}
						else
						{
							sb.Append(GeneratePageLink(i.ToString(), i));
						}
					}

					sb.Append("<div class=\"range\">...</div>");
					sb.Append(GeneratePageLink(pageCount.ToString(), pageCount));
				}
				else if (pageCount - currentPage < INITIAL_PAGE_LIMIT - 1)
				{
					sb.Append(GeneratePageLink("1", 1));
					sb.Append("<div class=\"range\">...</div>");
					for (int i = pageCount - INITIAL_PAGE_LIMIT + 1; i <= pageCount; i++)
					{
						if (i == this.currentPage)
						{
							sb.AppendFormat("<div class=\"page current\">{0}</div>", i);
						}
						else
						{
							sb.Append(GeneratePageLink(i.ToString(), i));
						}
					}
				}
				else
				{
					sb.Append(GeneratePageLink("1", 1));
					sb.Append("<div class=\"range\">...</div>");
					for (int i = currentPage - 1; i <= currentPage + 1; i++)
					{
						if (i == this.currentPage)
						{
							sb.AppendFormat("<div class=\"page current\">{0}</div>", i);
						}
						else
						{
							sb.Append(GeneratePageLink(i.ToString(), i));
						}
					}
					sb.Append("<div class=\"range\">...</div>");
					sb.Append(GeneratePageLink(pageCount.ToString(), pageCount));
				}
			}
			else
			{
				for (int i = start; i <= end; i++)
				{
					if (i == this.currentPage)
					{
						sb.AppendFormat("<div class=\"page current\">{0}</div>", i);
					}
					else
					{
						sb.Append(GeneratePageLink(i.ToString(), i));
					}
				}
			}
			// Next
			if (this.currentPage < pageCount)
			{
				sb.Append(GeneratePageLink("<div><div>Next</div>&nbsp;<i class=\"fa fa-long-arrow-right\"></i></div>", (this.currentPage + 1)));
			}
			else
			{
				sb.Append("<div class=\"page disabled\"><div><div>Next</div>&nbsp;<i class=\"fa fa-long-arrow-right\"></i></div></div>");
			}
			return MvcHtmlString.Create(sb.ToString());
		}

		private string GeneratePageLink(string linkText, int pageNumber)
		{
			string linkFormat = $"<div class=\"page\" data-page=\"{pageNumber}\"><a href=\"javascript:void(0)\">{linkText}</a></div>";
			return linkFormat;
		}

	}
}