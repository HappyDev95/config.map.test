kubectl delete --all pods -n test-namespace
kubectl delete cm test-configmap -n test-namespace
kubectl delete service configmap-test-service -n test-namespace
kubectl delete ns test-namespace