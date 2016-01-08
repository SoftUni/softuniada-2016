FOR %%f in ("*.in.txt") DO (
	SETLOCAL EnableDelayedExpansion
    SET "file=%%f"
    ..\bin\Debug\Problem-9-Road-Rage.exe < "%%f" > "!file:.in.txt=.out.txt!"
)
