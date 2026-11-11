# Issue
I got an error during execution of GitHub Action continuous_integration_and_delivery.yml
while deploying to production (job deploy-to-prod). Problem: It ran concurrently,
so only one of them succeeded. The others ran to the error that the container app can not be 
deployed in the same timeframe.

# Expected solution:
Add a debouncing for production deployment in order the deployment to 
production can only be triggered once at a time.

# Actual solution:
We debounce the whole ci/cd script continuous_integration_and_delivery.yml.
It ensures that the action does never run in parallel and it is ensured, that the last triggered
one is executed. 

There is one downside: If more than 2 runs are triggered in parallel for the same branch, 
some of them will be skipped. For this project this is no problem