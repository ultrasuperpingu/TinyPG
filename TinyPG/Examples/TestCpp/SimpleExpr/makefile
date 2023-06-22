CC=g++
CFLAGS=-c -Wall
LDFLAGS=
SOURCES=main.cpp src/Parser.cpp src/ParseTree.cpp src/Scanner.cpp
OBJECTS=$(SOURCES:.cpp=.o)
EXECUTABLE=simpleExpr

all: $(SOURCES) $(EXECUTABLE)

$(EXECUTABLE): $(OBJECTS) 
	$(CC) $(LDFLAGS) $(OBJECTS) -o $@ -std=c++17

.cpp.o:
	$(CC) $(CFLAGS) $< -o $@ -Iinclude/ -std=c++17
	
clean:
	rm *.o src/*.o simpleExpr