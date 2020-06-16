# FoodTruckRetriever

Food Truck Retriever is a .NET Core console app that retrieves open food truck data at the current time within an area. The area is circle that is defined by user input. The inputs are origin latitude, origin longitude, and range. This app currently only works for the San Francisco area. It is leverageing data from [San Francisco Mobile Food Schedule.](https://data.sfgov.org/Economy-and-Community/Mobile-Food-Schedule/jjew-r69b)

# Prerequisites and Setup
In order for the application to be able to access the San Francisco Mobile Food Schedule the user must procure a App Token. To obtain an App Token please visit [here](https://data.sfgov.org/profile/edit/developer_settings). After procuring an app token the user needs to update the appsettings.json found in the FoodTruckRetriever.Console project with the app token. A sample of the appsettings.json is provided below.

    {
    "WebSettingsConfiguration": 
        {
        "Schema": "https",
        "HostName": "data.sfgov.org",
        "Path": "resource/jjew-r69b.json",
        "AppToken": "Your_Key_Goes_Here"
        }
    }

# Running via Docker Container
The app can be ran from Docker. Setting Docker to Linux containers run the following commands from the command line in the directoy that contains the Dockerfile file:

    \FoodTruckRetriever>docker build -t foodtruckretriever .
    \FoodTruckRetriever>docker run -i foodtruckretriever

# Using the Application
A sample prompt and user input is provided below:

    Please enter your Latitude
    37.777261
    Please enter your Longitude
    -122.416225
    Please enter the range in miles you want to search within
    20 
