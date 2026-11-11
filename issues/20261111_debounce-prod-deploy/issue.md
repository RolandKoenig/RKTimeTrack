I got an error during execution of GitHub Action continuous_integration_and_delivery.yml
while deploying to production (job deploy-to-prod). Problem: It ran concurrently,
so only one of them succeeded. The others ran to the error that the container app can not be 
deployed in the same timeframe.

expected solution:
Add a debouncing for production deployment in order the deployment to 
production can only be triggered once at a time.