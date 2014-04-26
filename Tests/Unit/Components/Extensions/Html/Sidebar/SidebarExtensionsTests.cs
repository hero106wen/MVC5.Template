﻿using NUnit.Framework;
using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Template.Components.Extensions.Html;
using Template.Objects;
using Template.Tests.Helpers;

namespace Template.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class SidebarExtensionsTests
    {
        private HtmlHelper html;

        [SetUp]
        public void SetUp()
        {
            html = new HtmlMock().Html;
            html.ViewContext.HttpContext.Request.RequestContext.RouteData.Values["controller"] = "Roles";
            html.ViewContext.HttpContext.Request.RequestContext.RouteData.Values["area"] = "Administration";
        }

        #region Extension method: SidebarSearch(this HtmlHelper html)

        [Test]
        public void SidebarSearch_FormsSidebarSearch()
        {
            String actual = html.SidebarSearch().ToString();
            String expected = String.Format("<input id=\"SearchInput\" placeholder=\"{0}...\" type=\"text\" />",
                Template.Resources.Shared.Resources.Search);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: SidebarMenu(this HtmlHelper html, IEnumerable<Menu> menus)

        [Test]
        public void SidebarMenu_FormsSidebarMenu()
        {
            MenuFactory menuFactory = new MenuFactory(html.ViewContext.HttpContext);

            String actual = html.SidebarMenu().ToString();
            String expected = String.Empty;
            
            foreach (Menu menu in menuFactory.GetAuthorizedMenus())
                expected += Menu(html, menu);

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        #endregion

        #region Test helpers

        private String Menu(HtmlHelper html, Menu menu)
        {
            TagBuilder menuItem = new TagBuilder("li");
            StringBuilder menuInnerHtml = new StringBuilder();
            menuInnerHtml.Append(FormAction(html, menu));

            if (menu.Submenus.Count() > 0)
            {
                menuItem.AddCssClass("submenu");
                TagBuilder submenus = new TagBuilder("ul");
                StringBuilder innerSubmenuHtml = new StringBuilder();
                foreach (Menu submenu in menu.Submenus)
                    innerSubmenuHtml.Append(Menu(html, submenu));

                submenus.InnerHtml = innerSubmenuHtml.ToString();
                menuInnerHtml.Append(submenus);
            }

            if (menu.HasActiveChild) menuItem.AddCssClass("has-active-child open");
            if (menu.IsActive) menuItem.AddCssClass("active active-hovering");
            menuItem.InnerHtml = menuInnerHtml.ToString();
            return menuItem.ToString();
        }
        private String FormAction(HtmlHelper html, Menu menu)
        {
            TagBuilder menuIcon = new TagBuilder("i");
            TagBuilder span = new TagBuilder("span");
            menuIcon.AddCssClass(menu.IconClass);
            span.InnerHtml = menu.Title;

            if (menu.Action == null)
            {
                TagBuilder openIcon = new TagBuilder("i");
                TagBuilder action = new TagBuilder("a");

                action.InnerHtml = String.Format("{0}{1}{2}", menuIcon, span, openIcon);
                openIcon.AddCssClass("arrow fa fa-chevron-right");

                return action.ToString();
            }

            return String.Format(
                html.ActionLink(
                    "{0}{1}",
                    menu.Action,
                    new
                    {
                        area = menu.Area,
                        controller = menu.Controller
                    }).ToString(),
                menuIcon, span);
        }

	    #endregion
    }
}
