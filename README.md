# react-dot-net-core-weather-app

## SetUp

1. Register account in https://openweathermap.org/
2. Add key `PeriodicWeather:ApiKey` in `WeatherApp\appsettings.json`.
3. Set database connection `DatabaseConnection` in `WeatherApp\appsettings.json`.
4. Run `npm install` in `WeatherApp\ClientApp`.
5. Run application with Visual Studio, with every minute logs should be stored to database.
6. Refresh page to see Charts which represents lowest temperature and highest wind speed for 10 cities from 5 countries.