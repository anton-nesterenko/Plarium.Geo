Plarium.Geo
=========

Lightweight GeoIP library to determine the country, dial-code, timezone associated with IPv4 addresses. It uses the MaxMind GeoIP2 (free) database which needs to be downloaded using embedded updater.

Installation
-------

## To install Plarium.Geo, run the following command in the Nuget Package Manager Console

    PM> Install-Package Plarium.Geo

## To install Plarium.Geo.Embedded, run the following command in the Nuget Package Manager Console

    PM> Install-Package Plarium.Geo.Embedded


Usage Plarium.Geo
-------

* You can update database file manually at any time
```csharp
// using default settings
GeoUpdater.Update();
// forced update
GeoUpdater.Update(mode: GeoUpdater.UpdateMode.Lite, force: true);
```

* Example of update of GeoService with database file:
```csharp
GeoUpdater.Update();
var builder = new GeoServiceBuilder();
var service = new GeoService(builder);

var ipAddress = "2607:f0d0:1002:51::4";

service.ResolveCountry(ipAddress);
service.ResolveDialCodeByIP(ipAddress);
service.ResolveTimezone(ipAddress);
```

Usage Plarium.Geo.Embedded
-------

* Example of update of GeoService with embedded resources:
```csharp
var builder = new GeoServiceBuilder();
builder.RegisterResource<EmbeddedResourceReader>();
var service = new GeoService(builder);

var ipAddress = "2607:f0d0:1002:51::4";

service.ResolveCountry(ipAddress);
service.ResolveDialCodeByIP(ipAddress);
service.ResolveTimezone(ipAddress);

History
-------

TODO: Write history

Code Status
-------

TODO: Write Code Status

License
-------

The following source/framework has been applied with the [MIT license](https://github.com/anton-nesterenko/Plarium.Geo/LICENSE)





