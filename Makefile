quetzalDragon.exe: Driver.cs Scanner.cs Token.cs TokenCategory.cs
	
	mcs -out:quetzalDragon.exe Driver.cs Scanner.cs Token.cs TokenCategory.cs

celan:
	rm -f quetzalDragon.exe