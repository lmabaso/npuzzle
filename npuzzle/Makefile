exec:
	mono npuzzle.exe

compile: \
npuzzle.cs
	@mcs npuzzle.cs
	@echo "compiling"
	@touch compile

clean:
	echo "cleaning"
	@rm *.exe
	@rm compile

re: clean compile
