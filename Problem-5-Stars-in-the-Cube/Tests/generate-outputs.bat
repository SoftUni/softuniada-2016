FOR %%f in ("*.in.txt") DO (
	SETLOCAL EnableDelayedExpansion
    SET "file=%%f"
    ..\bin\Debug\Stars-in-the-Cube.exe < "%%f" > "!file:.in.txt=.out.txt!"
)
