FOR %%f in ("*.in.txt") DO (
	SETLOCAL EnableDelayedExpansion
    SET "file=%%f"
    ..\bin\Debug\Problem-4-Crossing-Figures.exe < "%%f" > "!file:.in.txt=.out.txt!"
)
