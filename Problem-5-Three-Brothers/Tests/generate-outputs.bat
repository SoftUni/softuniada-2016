FOR %%f in ("*.in.txt") DO (
	SETLOCAL EnableDelayedExpansion
    SET "file=%%f"
    ..\bin\Debug\Three-Brothers-Dynamic.exe < "%%f" > "!file:.in.txt=.out.txt!"
)
