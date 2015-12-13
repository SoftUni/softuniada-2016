FOR %%f in ("*.in.txt") DO (
	SETLOCAL EnableDelayedExpansion
    SET "file=%%f"
    ..\bin\Debug\Groups-Of-Equal-Sum.exe < "%%f" > "!file:.in.txt=.out.txt!"
)
