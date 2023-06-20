CC=g++
CFLAGS=-c
LDFLAGS=
SOURCES=src/main.cpp src/Context.cpp src/DynamicFunction.cpp src/Functions.cpp src/Parser.cpp src/ParseTree.cpp src/Scanner.cpp src/StaticFunction.cpp src/Variables.cpp
OBJECTS=$(SOURCES:.cpp=.o)
EXECUTABLE=tinyexe

all: $(SOURCES) $(EXECUTABLE)

$(EXECUTABLE): $(OBJECTS) 
	$(CC) $(LDFLAGS) $(OBJECTS) -o $@ -std=c++17

.cpp.o:
	$(CC) $(CFLAGS) $< -o $@ -Iinclude/ -std=c++17
	
clean:
	rm src/*.o tinyexe