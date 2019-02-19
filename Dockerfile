FROM microsoft/dotnet:2.2-sdk-alpine

ARG CI
ARG TRAVIS
ARG TRAVIS_BRANCH
ARG TRAVIS_COMMIT
ARG TRAVIS_JOB_ID
ARG TRAVIS_JOB_NUMBER
ARG TRAVIS_OS_NAME
ARG TRAVIS_PULL_REQUEST
ARG TRAVIS_PULL_REQUEST_SHA
ARG TRAVIS_REPO_SLUG
ARG TRAVIS_TAG

ARG SKIP_TEST
ARG SKIP_COVERALLS

ENV PATH="${PATH}:/root/.dotnet/tools"

WORKDIR /app
ADD . /app

# test
RUN if [ -z "$SKIP_TEST" ]; then \
  cd /app/Bynder/Test/ && dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover && cd /app; \
  else echo "skip test"; fi

# submit coverage to coverals if COVERALLS_TOKEN in env
RUN if [ -z "$SKIP_COVERALLS" ] && [ -z "$SKIP_TEST" ]; then \
  dotnet tool install -g coveralls.net && csmacnz.Coveralls --opencover -i /app/Bynder/Test/coverage.opencover.xml --useRelativePaths || echo "coverall failed!"; \
  else echo "skip coveralls"; fi
