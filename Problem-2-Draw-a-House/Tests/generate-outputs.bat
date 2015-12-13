FOR %%f in ("*.in.txt") DO (
	SETLOCAL EnableDelayedExpansion
    SET "file=%%f"
    ..\bin\Debug\Draw-a-House.exe < "%%f" > "!file:.in.txt=.out.txt!"
)
