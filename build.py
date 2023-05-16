import os
import platform
import subprocess
from ninja_generator import *

generate_build_ninja()
subprocess.run(["ninja"])
archive_artifacts()

