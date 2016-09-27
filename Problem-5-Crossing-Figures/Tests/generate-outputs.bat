FOR %%f in ("*.in.txt") DO (
	SETLOCAL EnableDelayedExpansion
    SET "file=%%f"
    ..\bin\Debug\Problem-5-Crossing-Figures.exe < "%%f" > "!file:.in.txt=.out.txt!"
)
