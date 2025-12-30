echo "Stopping Production environment for LouisVenhoff Portfolio"

STACK_NAME="portfolio"
IMAGE_NAME="portfolio_app"


if docker info 2>/dev/null | grep -q "Swarm: active"; then
    echo "✔ Active Swarm found"
else
    echo "⚠ System already Stopped"
    exit 0
fi

if docker stack ls --format '{{.Name}}' | grep -qx "$STACK_NAME"; then
  echo "ℹ Deleting stack"

  docker stack rm $STACK_NAME
fi

echo "ℹ Removing swarm"

docker swarm leave --force

if docker image ls | grep -q "$IMAGE_NAME"; then
    echo "ℹ Deleting outdated image"

    docker rmi $IMAGE_NAME
fi

echo "✔ Stopped!"