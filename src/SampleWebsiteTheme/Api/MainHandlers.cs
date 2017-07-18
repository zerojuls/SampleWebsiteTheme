using System.IO;
using System.Linq;
using Simplified.Ring1;
using Simplified.Ring6;
using Starcounter;

namespace SampleWebsiteTheme.Api
{
    public class MainHandlers
    {
        private static string DefaultSystemLogoName = "Default system logo";
        private static string DefaultSystemLogoContentName = "Default system logo image";
        private static string DefaultSystemLogoContentUrl = "/samplewebsitetheme/img/logo.png";
        private static string DefaultSystemLogoContentMimeType = "image/png";

        public void Run()
        {
            RegisterHandlers();
            CreateLayout();
        }

        private static void CreateLayout()
        {
            Db.Transact(() => {
                RemoveAllDefaultCatchUris();
                CreateLoginLayout();
                CreateDesktopLayout();
            });
        }

        private static void RemoveAllDefaultCatchUris()
        {
            var defaultCatchUris = Db.SQL<WebUrl>(
                $"select a from {typeof(WebUrl).FullName} a where {nameof(WebUrl.Url)} is null and {nameof(WebUrl.IsFinal)} = ?",
                true);
            foreach (WebUrl catchUri in defaultCatchUris)
            {
                catchUri.Url = "Removed by SampleWebsiteTheme";
            }
        }

        private static void CreateLoginLayout()
        {
            WebTemplate loginSurface = CreateSurface("LoginSurface", "LoginSurface");

            CreateCatchUri(loginSurface, "/signin/signinuser");
            WebSection logoBp = CreateBlendingPoint(loginSurface, "Logo");
            WebSection footerBp = CreateBlendingPoint(loginSurface, "Footer");
            CreateBlendingPoint(loginSurface, "SignIn", isBlendingPointDefault: true);

            CreatePinningRule(logoBp, CreateWrappedHtmlUrl("Logo"), 0);
            CreatePinningRule(logoBp, CreateWrappedHtmlUrl("Slogan"), 1);
            CreatePinningRule(footerBp, CreateWrappedHtmlUrl("TermsAndConditionsLink"));

            var compositionKey = "/sc/htmlmerger?SignIn=/SignIn/viewmodels/SignInFormPage.html";
            var composition = HTMLComposition.GetUsingKey(compositionKey) ?? new HTMLComposition()
                {
                    Key = compositionKey
                };
            // ReSharper disable once AssignNullToNotNullAttribute this should always work, because it's compiled into the assembly
            using (var streamReader = new StreamReader(typeof(MainHandlers).Assembly.GetManifestResourceStream("SampleWebsiteTheme.wwwroot.SampleWebsiteTheme.html.SignInComposition.html")))
            {
                composition.Value = streamReader.ReadToEnd();
            }
        }

        private static void CreateDesktopLayout()
        {
            WebTemplate desktopSurface = CreateSurface("DesktopSurface", "DesktopSurface");

            CreateCatchUri(desktopSurface, null);

            WebSection topBarLeftBlendingPoint = CreateBlendingPoint(desktopSurface, "TopBarLeft");
            WebSection topBarLeftCornerBlendingPoint = CreateBlendingPoint(desktopSurface, "TopBarLeftCorner");
            WebSection topBarCenterBlendingPoint = CreateBlendingPoint(desktopSurface, "TopBarCenter");
            WebSection topBarRightBlendingPoint = CreateBlendingPoint(desktopSurface, "TopBarRight");
            CreateBlendingPoint(desktopSurface, "Main", isBlendingPointDefault: true);

            var content = Db.SQL<Content>($"SELECT c FROM {typeof(Content).FullName} c WHERE {nameof(Content.URL)} = ?", DefaultSystemLogoContentUrl).First;
            if (content == null)
            {
                content = new Content()
                {
                    Name = DefaultSystemLogoContentName,
                    URL = DefaultSystemLogoContentUrl,
                    MimeType = DefaultSystemLogoContentMimeType
                };
            }

            var concept = Db.SQL<Something>($"SELECT s FROM {typeof(Something).FullName} s WHERE {nameof(Something.Name)} = ?", DefaultSystemLogoName).First;
            if (concept == null)
            {
                concept = new Something()
                {
                    Name = DefaultSystemLogoName
                };
            }

            if (concept.Illustration == null)
            {
                new Illustration()
                {
                    WhatIs = content,
                    ToWhat = concept
                };
            }

            CreatePinningRule(topBarCenterBlendingPoint, "/Search");
            CreatePinningRule(topBarCenterBlendingPoint, $"/images/partials/somethings-single-static/{concept.GetObjectID()}");
            CreatePinningRule(topBarLeftBlendingPoint, "/DisplayAppTitle");
            CreatePinningRule(topBarLeftCornerBlendingPoint, "/Launchpad");
            CreatePinningRule(topBarRightBlendingPoint, "/signin/user");
        }

        private static WebTemplate CreateSurface(string surfaceName, string templateFileName)
        {
            var surface = Db.SQL<WebTemplate>(
                                  $"select a from {typeof(WebTemplate).FullName} a where {nameof(WebTemplate.Name)} = ?",
                                  surfaceName)
                              .FirstOrDefault()
                          ?? new WebTemplate() {Name = surfaceName};
            surface.Html = CreateStaticHtmlUrl(templateFileName);
            return surface;
        }

        private static WebMap CreatePinningRule(WebSection blendingPoint, string foreignUrl, int order = 0)
        {
            var pinningRule = blendingPoint.Maps.FirstOrDefault(map => map.Url == null && map.ForeignUrl == foreignUrl && map.Section.Key == blendingPoint.Key)
                              ?? new WebMap() {
                                  Section = blendingPoint,
                                  Url = null,
                                  ForeignUrl = foreignUrl
                              };
            pinningRule.SortNumber = order;
            return pinningRule;
        }

        private static WebUrl CreateCatchUri(WebTemplate surface, string uri)
        {
            var catchUri = surface.Pages
                               .FirstOrDefault(url => url.Template.Key == surface.Key) ?? new WebUrl()
                           {
                               Template = surface,
                           };
            catchUri.Url = uri;
            catchUri.IsFinal = true;
            return catchUri;
        }

        private static WebSection CreateBlendingPoint(
            WebTemplate surface,
            string blendingPointName,
            bool isBlendingPointDefault = false)
        {
            var blendingPoint = surface.Sections
                .FirstOrDefault(section => section.Name == blendingPointName)
                ?? new WebSection()
                {
                    Name = blendingPointName,
                    Template = surface
                };
            blendingPoint.Default = isBlendingPointDefault;
            return blendingPoint;
        }

        private static string CreateStaticHtmlUrl(string fileName) => $"/samplewebsitetheme/html/{fileName}.html";
        private static string CreateWrappedHtmlUrl(string fileName) => $"/samplewebsitetheme/html/dynamic/{fileName}.html";

        private static void RegisterHandlers()
        {
            Application.Current.Use(new HtmlFromJsonProvider());
            Handle.GET("/samplewebsitetheme", () => Self.GET(CreateStaticHtmlUrl("LandingPage")));
            Handle.GET<string>("/samplewebsitetheme/html/dynamic/{?}", file => new Json {["Html"] = $"/samplewebsitetheme/html/{file}"});
        }
    }
}
