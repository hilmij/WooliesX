## Challenges
### Challenge 1
http://functionapp20180415064436.azurewebsites.net/api/answers

### Challenge 2
http://functionapp20180415064436.azurewebsites.net/api/sort/{sortOption?}

Notes:
- Recommended sort is failing from the test application, but I think the function returns the correct sorted products as per my understanding.

### Challenge 3
http://functionapp20180415064436.azurewebsites.net/api/trolleyCalculator
http://functionapp20180415064436.azurewebsites.net/api/trolleyCalculator/trolleyTotal

Notes:
- There is an issue in the test app. Instead of calling "trolleyCalculator" it calls "trolleyCalculator/trolleyTotal". The function app implements both the routes as a workaround.

## Technologies
- C#, Azure Functions.

## Other Notes
- Use Visual Studio 2017.

