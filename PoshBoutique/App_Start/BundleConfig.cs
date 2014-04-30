﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace PoshBoutique
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/angular.min.js",
                "~/Scripts/angular-ui-router.js",
                "~/Scripts/angular-tree.js",
                "~/Scripts/ngprogress-lite.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/app/utils.js",
                "~/Scripts/app/poshBoutiqueApp.js",
                "~/Scripts/app/controllers/homeController.js",
                "~/Scripts/app/controllers/catalogueController.js",
                "~/Scripts/app/controllers/autherrorController.js",
                "~/Scripts/app/directives/productsList.js",
                "~/Scripts/app/directives/productItem.js",
                "~/Scripts/app/directives/imagesView.js",
                "~/Scripts/app/directives/magnifyable.js",
                "~/Scripts/app/directives/configurePurchaseForm.js",
                "~/Scripts/app/directives/repeatPassword.js",
                "~/Scripts/app/services/singleProductModal.js",
                "~/Scripts/app/services/detailsViewParentState.js",
                "~/Scripts/app/services/dataServices.js",
                "~/Scripts/app/services/articleListParams.js",
                "~/Scripts/app/services/accountDataService.js",
                "~/Scripts/app/services/authenticateModal.js",
                "~/Scripts/app/services/authenticationStorage.js",
                "~/Scripts/app/services/currentUser.js"
                ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js",
                "~/Scripts/ui-bootstrap-tpls-0.10.0.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/bootstrap.css",
                 "~/Content/Site.css",
                 "~/Content/ngprogress-lite.css"));
        }
    }
}