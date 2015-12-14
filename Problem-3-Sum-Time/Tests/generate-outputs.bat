FOR %%f in ("*.in.txt") DO (
	SETLOCAL EnableDelayedExpansion
    SET "file=%%f"
    ..\bin\Debug\Sum-Time.exe < "%%f" > "!file:.in.txt=.out.txt!"
)
