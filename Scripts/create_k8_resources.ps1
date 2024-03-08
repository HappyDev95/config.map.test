kubectl create ns test-namespace
kubectl create -f configmap.yaml -n test-namespace
kubectl create -f createservice.yaml -n test-namespace
kubectl create -f createpod.yaml -n test-namespace