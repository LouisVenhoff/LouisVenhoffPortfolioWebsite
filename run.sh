echo "Setting up Production environment for LouisVenhoff Portfolio"

echo "ℹ Updating sources"

git pull

if docker info 2>/dev/null | grep -q "Swarm: active"; then
    echo "✔ Swarm found"
else
    echo "⚠ Please initialize a swarm on this device!"
    exit 0
fi

STACK_NAME="portfolio"
IMAGE_NAME="portfolio_app"

if docker stack ls --format '{{.Name}}' | grep -qx "$STACK_NAME"; then
  echo "ℹ Deleting old stack"

  docker stack rm portfolio
fi

if docker image ls | grep -q "$IMAGE_NAME"; then
    echo "ℹ Deleting outdated image"
fi

echo "ℹ Building new image"

docker build -t "portfolio_app" .

echo "ℹ Deploying services to swarm"

docker stack deploy -c docker-swarm.yml portfolio

echo "✔  Build successfull"