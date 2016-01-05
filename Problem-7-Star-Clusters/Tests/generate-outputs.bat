FOR %%f in ("*.in.txt") DO (
	SETLOCAL EnableDelayedExpansion
    SET "file=%%f"
    ..\bin\Debug\Problem-7-Star-Clusters.exe < "%%f" > "!file:.in.txt=.out.txt!"
)
