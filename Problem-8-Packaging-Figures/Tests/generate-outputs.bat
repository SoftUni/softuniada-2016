FOR %%f in ("*.in.txt") DO (
	SETLOCAL EnableDelayedExpansion
    SET "file=%%f"
    ..\bin\Debug\Problem-8-Packaging-Figures.exe < "%%f" > "!file:.in.txt=.out.txt!"
)
