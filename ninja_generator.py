import os
import platform
import subprocess
import zipfile
import shutil
import glob

class WindowsProject:
    def __init__(self, name, path, configs=["Debug", "Release"], platforms=["x86", "x64"], artifacts=["__PLATFORM__/__CONFIG__/*.dll"]):
        self.name = name
        self.path = path
        self.configs = configs
        self.platforms = platforms
        self.artifacts = artifacts

projects = [
    WindowsProject("GLideN64", "GLideN64/projects/msvc/GLideN64.sln", configs=["Debug_mupenplus", "Release_mupenplus"], platforms=["Win32", "x64"], artifacts=["bin/__PLATFORM__/__CONFIG__/*.dll"]),
    WindowsProject("Modloader64", "Modloader64/Modloader64.csproj", artifacts=["bin/__PLATFORM__/__CONFIG__/net7.0/*"]),
    WindowsProject("mupen64plus-audio-sdl", "mupen64plus-audio-sdl/projects/msvc/mupen64plus-audio-sdl.vcxproj"),
    WindowsProject("mupen64plus-input-sdl", "mupen64plus-input-sdl/projects/msvc/mupen64plus-input-sdl.vcxproj"),
    WindowsProject("mupen64plus-rsp-hle", "mupen64plus-rsp-hle/projects/msvc/mupen64plus-rsp-hle.vcxproj"),
    WindowsProject("mupen64plus-core", "mupen64plus-core/projects/msvc/mupen64plus-core.sln",
            configs=["Debug", "Release"]),
]

def get_msbuild_path():
    vswhere = os.path.join(os.environ.get('ProgramFiles(x86)'), r"Microsoft Visual Studio\Installer\vswhere.exe")
    command = [vswhere,
               "-latest",
               "-products", "*",
               "-requires", "Microsoft.Component.MSBuild",
               "-property", "installationPath"]
    vs_path = subprocess.check_output(command).decode().strip()
    msbuild_path = os.path.join(vs_path, "MSBuild", "Current", "Bin", "MSBuild.exe")
    return msbuild_path

def generate_build_ninja_windows():
    msbuild = get_msbuild_path()

    # Generate the Ninja build file
    with open("build.ninja", "w") as f:
        f.write(f"# Autogenerated build.ninja\n\n")
        f.write(f"msbuild = {msbuild}\n\n")
        f.write(f"rule build_project\n")
        f.write(f"  command = $msbuild $in -nologo -t:Clean;Build /p:Configuration=$cfg /p:Platform=$platform /p:VisualStudioVersion=17.0\n")
        f.write(f"  description = Building $in...\n\n")

        all_targets = []

        for project in projects:
            last_target = ""
            for platform in project.platforms:
                for cfg in project.configs:
                    target_name = f"{project.name}_{platform}_{cfg}"
                    all_targets.append(target_name)

                    prereq = ""
                    if last_target != "":
                        prereq = f" | {last_target}"

                    f.write(f"build {target_name}: build_project {project.path}{prereq}\n")
                    f.write(f"  cfg={cfg}\n")
                    f.write(f"  platform={platform}\n\n")
                    last_target = target_name
                    
            f.write("\n")

        f.write(f"build all: phony {' '.join(all_targets)}\n")
        f.write(f"default all\n\n")

def generate_build_ninja_linux():
    print("TODO")

def generate_build_ninja():
    if platform.system() == "Windows": generate_build_ninja_windows()
    else: generate_build_ninja_linux()

def archive_artifacts():
    for project in projects:
        print(f"Archiving {project.name} ...")
        for platform in project.platforms:
            for cfg in project.configs:
                os.makedirs("bin", exist_ok=True)
                archive_name = f"{project.name}_{platform}_{cfg}"
                temp_folder = f"temp_{archive_name}"
                os.makedirs(temp_folder, exist_ok=True)
                for artifact in project.artifacts:
                    artifact = artifact.replace('__PLATFORM__', platform)
                    artifact = artifact.replace('__CONFIG__', cfg)
                    artifact = os.path.join(os.path.dirname(project.path), artifact)
                    # use glob to expand wildcard paths
                    for filename in glob.glob(artifact):
                        # copy the file to a temporary location
                        shutil.copy(filename, f"{temp_folder}/{os.path.basename(filename)}")

                # use 7z to create the archive
                archive_filename = os.path.join("bin", f"{archive_name}.zip")
                cwd = os.getcwd()
                os.chdir(temp_folder)
                subprocess.check_call(['7z', 'a', os.path.join(cwd, archive_filename), '*'])
                os.chdir(cwd)

                # remove the temporary folder
                shutil.rmtree(temp_folder)


