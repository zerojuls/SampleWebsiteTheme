# SampleWebsiteTheme

## Purpose of this app

This app creates the database configuration for best experience of using Starcounter Apps. 

## Features

The configuration consists of the following things:

1. Desktop Surface:
    - There is a menu button in the top left corner. Clicking on it shows the response from the [Launchpad](https://github.com/StarcounterApps/Launchpad) app, if that app is running.
    - There is a logo in the top bar. The logo comes from the [Images](https://github.com/StarcounterApps/Images) app, if that app is running. More info about the logo [below](#How-the-logo-is-maintained).
2. Login Surface:
    - Automatically presented when the user is not signed in. Shows the response from the [Sign In](https://github.com/StarcounterApps/SignIn) app, if that app is running.

Planned additional surface:

3. Mobile Surface:
    - Similar to Desktop but optimized for mobile devices

## Setup

Requires Starcounter 2.3.0.6067 or newer and the apps in the following vesions (17 May 2017):

- https://github.com/StarcounterApps/Website/tree/develop  
- https://github.com/StarcounterApps/SignIn/tree/develop  
- https://github.com/StarcounterApps/Launchpad/tree/develop  
- https://github.com/StarcounterApps/Images/tree/develop

To try it out, run the following apps:

```batch
call samplewebsitetheme\run.bat
call signin\run.bat
call website\run.bat
call launchpad\run.bat
call images\run.bat
```

# How the logo is maintained

The logo in the Desktop Surface comes from the Images app.

Desktop Surface is configured to create the data for it (`Content`, `Illustration`, `Something` objects) and the static file for the default logo (`logo.png`).

To display the logo, the partial view `/images/partials/somethings-single-static/{Something ObjectID}` from the Images app is pinned to the "Logo" blending point of the Desktop Surface.

To change the logo, you need to run go to the Images list [http://localhost:8080/images](http://localhost:8080/images), click the "Edit" button next to the current logo image and upload a new image. Then reload the page.