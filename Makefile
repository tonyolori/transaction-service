# Makefile for Transaction Service

SOLUTION=./src/TransactionService.sln

API=./src/TransactionService.Api
WORKER=./src/TransactionService.Worker
GRPC=./src/TransactionService.Grpc

LOG_DIR=./logs
API_LOG=$(LOG_DIR)/api.log
WORKER_LOG=$(LOG_DIR)/worker.log
GRPC_LOG=$(LOG_DIR)/grpc.log

# Colors
BLUE=$$(printf '\033[34m')
GREEN=$$(printf '\033[32m')
MAGENTA=$$(printf '\033[35m')
RESET=$$(printf '\033[0m')

.PHONY: run-api run-worker run-grpc run-all \
        watch-api watch-worker watch-grpc watch-all \
        stop-all build clean restore logs tail-api tail-worker tail-grpc tail-all

## Ensure logs directory exists
logs:
	mkdir -p $(LOG_DIR)

## Run API (logs with prefix)
run-api: logs
	dotnet run --project $(API) 2>&1 | sed "s/^/[$(BLUE)API$(RESET)] /" | tee $(API_LOG)

## Run Worker (logs with prefix)
run-worker: logs
	dotnet run --project $(WORKER) 2>&1 | sed "s/^/[$(GREEN)WORKER$(RESET)] /" | tee $(WORKER_LOG)

## Run gRPC (logs with prefix)
run-grpc: logs
	dotnet run --project $(GRPC) 2>&1 | sed "s/^/[$(MAGENTA)GRPC$(RESET)] /" | tee $(GRPC_LOG)

## Run all services with colored prefixes and proper signal handling
run-all: logs
	@echo "Starting all services... Press Ctrl+C to stop all"
	@trap 'echo ""; echo "Stopping all services..."; $(MAKE) stop-all; exit 0' INT TERM; \
	( \
		( stdbuf -i0 -o0 -e0 dotnet run --project $(API) 2>&1 | \
		  stdbuf -i0 -o0 -e0 sed -u "s/^/[$(BLUE)API$(RESET)] /" | \
		  tee $(API_LOG) ) & \
		API_PID=$$!; \
		( stdbuf -i0 -o0 -e0 dotnet run --project $(WORKER) 2>&1 | \
		  stdbuf -i0 -o0 -e0 sed -u "s/^/[$(GREEN)WORKER$(RESET)] /" | \
		  tee $(WORKER_LOG) ) & \
		WORKER_PID=$$!; \
		( stdbuf -i0 -o0 -e0 dotnet run --project $(GRPC) 2>&1 | \
		  stdbuf -i0 -o0 -e0 sed -u "s/^/[$(MAGENTA)GRPC$(RESET)] /" | \
		  tee $(GRPC_LOG) ) & \
		GRPC_PID=$$!; \
		echo "Services started - API: $$API_PID, Worker: $$WORKER_PID, gRPC: $$GRPC_PID"; \
		wait \
	)

## Alternative run-all using multitail (install with: brew install multitail or apt-get install multitail)
run-all-multitail: logs
	@echo "Starting all services with multitail..."
	@trap '$(MAKE) stop-all; exit 0' INT TERM; \
	( \
		stdbuf -i0 -o0 -e0 dotnet run --project $(API) 2>&1 | \
		stdbuf -i0 -o0 -e0 sed -u "s/^/[API] /" > $(API_LOG) \
	) & \
	( \
		stdbuf -i0 -o0 -e0 dotnet run --project $(WORKER) 2>&1 | \
		stdbuf -i0 -o0 -e0 sed -u "s/^/[WORKER] /" > $(WORKER_LOG) \
	) & \
	( \
		stdbuf -i0 -o0 -e0 dotnet run --project $(GRPC) 2>&1 | \
		stdbuf -i0 -o0 -e0 sed -u "s/^/[GRPC] /" > $(GRPC_LOG) \
	) & \
	sleep 2; \
	multitail -ci blue $(API_LOG) -ci green $(WORKER_LOG) -ci magenta $(GRPC_LOG)

## Simple run-all that outputs to separate terminals (requires tmux)
run-all-tmux: logs
	@echo "Starting all services in tmux session..."
	tmux new-session -d -s transaction-services
	tmux split-window -h -t transaction-services
	tmux split-window -v -t transaction-services:0.1
	tmux send-keys -t transaction-services:0.0 'make run-api' Enter
	tmux send-keys -t transaction-services:0.1 'make run-worker' Enter
	tmux send-keys -t transaction-services:0.2 'make run-grpc' Enter
	tmux attach-session -t transaction-services

## Hot reload API
watch-api: logs
	dotnet watch --project $(API) run 2>&1 | sed "s/^/[$(BLUE)API$(RESET)] /" | tee $(API_LOG)

## Hot reload Worker
watch-worker: logs
	dotnet watch --project $(WORKER) run 2>&1 | sed "s/^/[$(GREEN)WORKER$(RESET)] /" | tee $(WORKER_LOG)

## Hot reload gRPC
watch-grpc: logs
	dotnet watch --project $(GRPC) run 2>&1 | sed "s/^/[$(MAGENTA)GRPC$(RESET)] /" | tee $(GRPC_LOG)

## Hot reload all services with better output handling
watch-all: logs
	@echo "Starting all services with hot reload... Press Ctrl+C to stop all"
	@trap 'echo ""; echo "Stopping all services..."; $(MAKE) stop-all; exit 0' INT TERM; \
	( \
		( stdbuf -i0 -o0 -e0 dotnet watch --project $(API) run 2>&1 | \
		  stdbuf -i0 -o0 -e0 sed -u "s/^/[$(BLUE)API$(RESET)] /" | \
		  tee $(API_LOG) ) & \
		( stdbuf -i0 -o0 -e0 dotnet watch --project $(WORKER) run 2>&1 | \
		  stdbuf -i0 -o0 -e0 sed -u "s/^/[$(GREEN)WORKER$(RESET)] /" | \
		  tee $(WORKER_LOG) ) & \
		( stdbuf -i0 -o0 -e0 dotnet watch --project $(GRPC) run 2>&1 | \
		  stdbuf -i0 -o0 -e0 sed -u "s/^/[$(MAGENTA)GRPC$(RESET)] /" | \
		  tee $(GRPC_LOG) ) & \
		wait \
	)

## Stop all running services (improved)
stop-all:
	@echo "Stopping all TransactionService processes..."
	-pkill -f "dotnet.*TransactionService" 2>/dev/null || true
	-pkill -f "dotnet run --project.*TransactionService" 2>/dev/null || true
	-pkill -f "dotnet watch --project.*TransactionService" 2>/dev/null || true
	@sleep 1
	@echo "✅ All services stopped."

## Live view of all logs with colors
live-logs:
	@echo "Showing live logs for all services (Ctrl+C to exit)..."
	@tail -f $(API_LOG) $(WORKER_LOG) $(GRPC_LOG) 2>/dev/null | \
	while IFS= read -r line; do \
		case "$$line" in \
			*api.log*) echo "$(BLUE)$$line$(RESET)" ;; \
			*worker.log*) echo "$(GREEN)$$line$(RESET)" ;; \
			*grpc.log*) echo "$(MAGENTA)$$line$(RESET)" ;; \
			*) echo "$$line" ;; \
		esac; \
	done

## Tail logs
tail-api:
	tail -f $(API_LOG)

tail-worker:
	tail -f $(WORKER_LOG)

tail-grpc:
	tail -f $(GRPC_LOG)

tail-all:
	tail -f $(API_LOG) $(WORKER_LOG) $(GRPC_LOG)

test:
	dotnet test ./src/TransactionService.sln \
		--collect:"XPlat Code Coverage" \
		--results-directory ./TestResults \
		/p:CollectCoverage=true \
		/p:CoverletOutput=./TestResults/coverage/ \
		/p:CoverletOutputFormat=lcov

coverage-report:
	reportgenerator -reports:./TestResults/*/coverage.cobertura.xml -targetdir:./TestResults/coverage-report -reporttypes:Html

## Build the solution
build:
	dotnet build $(SOLUTION)

## Clean the solution
clean:
	dotnet clean $(SOLUTION)

## Restore NuGet packages
restore:
	dotnet restore $(SOLUTION)

## Show help
help:
	@echo "Available commands:"
	@echo "  run-all          - Run all services with output streaming"
	@echo "  run-all-tmux     - Run all services in separate tmux panes"
	@echo "  watch-all        - Run all services with hot reload"
	@echo "  stop-all         - Stop all running services"
	@echo "  live-logs        - View live logs from all services"
	@echo "  tail-all         - Tail all log files"
	@echo "  build            - Build the solution"
	@echo "  clean            - Clean the solution"
	@echo "  restore          - Restore NuGet packages"