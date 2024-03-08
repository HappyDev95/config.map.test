# Config Map Test

This is a test application is a simple example of how a container running in a k8 pod interacts with a ConfigMap that is mounted as a volume on the pod 

# How to use

This example assumes that you have a local k8 cluster running. I am using Docker Desktops build in k8 funcationality.

## Steps:
1. Run the Docker_Build.sh script (this will build the container image)
2. Run the create_k8_resources.ps1 script in the /Scripts directory to create the k8 resources (namespace, service, pod)
3. Navigate to http://localhost:9654/swagger/index.html to view the swagger page
4. The /api/v1/GetContentFromConfigMap endpoint will print out hte contents of the k8 configmap
5. When youre done run the cleanup_k8_resources.ps1 script to delete the namespace

# Summary

With this project we've created a basic .NET application that will read in a file that is a mounted k8 configmap at the specified directory on the container.
The configured directory points to a mounted filepath on the container running our .NET application in the Pod we've created.
We've created a serivce to allow us to view this Pod from our browser.