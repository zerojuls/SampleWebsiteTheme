# SampleWebsiteTheme

Requires Starcounter 2.3.0.6067 or newer.

To try it out, run the following apps:

```batch
call samplewebsitetheme\run.bat
call signin\run.bat
call website\run.bat
call launchpad\run.bat
call images\run.bat
```

# Required versions of other apps

As of Apr 21, 2017:

https://github.com/StarcounterApps/SignIn/tree/develop  
https://github.com/StarcounterApps/Website/tree/develop  
https://github.com/StarcounterApps/Launchpad/tree/fix-auto-uris (https://github.com/StarcounterApps/Launchpad/tree/fix-auto-uris) 
https://github.com/StarcounterApps/Images/tree/center-image (https://github.com/StarcounterApps/Images/pull/94)

# How the logo is maintained

The logo in the Desktop Theme comes from the Images app.

Desktop Theme is configured to create the data for it (`Content`, `Illustration`, `Something` objects) and the static file for the default logo (`logo.png`).

To display the logo, the partial view `/images/partials/somethings-single-static/{Something ObjectID}` from the Images app is pinned to the "Logo" blending point of the Desktop Theme surface.

To change the logo, you need to run go to the Images list [http://localhost:8080/images](http://localhost:8080/images), click the "Edit" button next to the current logo image and upload a new image. Then reload the page.