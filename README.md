Plarium.Geo
=========

Lightweight GeoIP library to determine the country, dial-code, timezone associated with IPv4 addresses. It uses the MaxMind GeoIP2 (free) database which needs to be downloaded using embedded updater.

Installation
-------

TODO: Describe the installation process


Usage
-------

* updater usage
GeoUpdater.DefaultUpdate();

* client usage
var service = new GeoService();
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

The following source/framework has been applied with the [MIT license](https://github.com/anton-nesterenko/Plarium.Geo/blob/master/LICENSE)





