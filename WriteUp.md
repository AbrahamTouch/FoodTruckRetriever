# Introduction

For this project I decided to make a console app that would get the current food trucks open in an area defined by the user in San Francisco. The prompt suggested we use the [Mobile Food Facility Permit Data](https://data.sfgov.org/Economy-and-Community/Mobile-Food-Facility-Permit/rqzj-sfat). I chose instead to use a child dataset, the [Mobile Food Schedule](https://data.sfgov.org/Economy-and-Community/Mobile-Food-Schedule/jjew-r69b), that contained the schedule of food trucks as part of its data explicitly.  I leveraged the functionality of the API to support looking up in a region.

# Architecture/Design
The system is composed of the three primary components. The user interface, retrieving local San Francisco time, and retrieving Food Truck data.
The command line is used to input and output data with the user and serves as the user interface. FoodTruckRetriever is what interacts with the console. 
DateTime class in conjunction with TimeZoneConverter were used to get the current San Francisco Time. TimeZoneConverter was used because it supplied an OS agnostic way of getting a time zone info.
The repository pattern was leveraged in retrieving Food Truck data. This allows for easy testability and the Interface serves as an extension point. With that extension point we could introduce caching or add a database based repository. Most of the heavy lifting (business logic) is outsourced through the socrata web call. I tried to do as much filtering as possible at the API call to Mobile Food Schedule as possible while also retrieving only the pertinent sorted data for the application. The SOCRATA query is provided below for your reference. This is to minimize traffic and also acting under the assumption their system is highly optimized for such queries.

	$query=SELECT Applicant,location,end24,start24,Latitude,Longitude,optionaltext WHERE lower(DayOfWeekStr)=='currentDay' and within_circle(location_2,userInputLatitude,userInputLongitude,rangeInMeters) ORDER BY Applicant";

# Future Work
Due to a lack of time and tools the solution has poor test coverage, and exception handling is generic. I wanted to shim (Fake) a static method to isolate code in the repository class but since I don’t have an enterprise version of visual studio that option was not available to me. I could have factored out the method in question to a separate class/interface pair and mocked it but lack of time precluded that option as well.The exception handling in the code is generic. Given time it should handle specific exception cases with prompts to the user indicating what the issue is specifically. 

The solution lacks logging. The solutions lack of logging was due to lack of time. .NET Core provides a nice logging interface that can work with a multitude of loggers (log4net, console) and with DI the logger can be easily injected anywhere its needed. Logging is crucial to production systems as they help us understand our applications (Performance, Correctness, Usage statistics) and it was not omitted lightly.

There also is a limitation to the range that can be entered into the program. This limitation is based on our dependence on the Socrata API. The Socrata API does not have any documentation on what the max range is to search for is. Currently the program returns no trucks if the range is too high.



