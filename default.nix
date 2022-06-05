{ lib, buildDotnetModule, dotnetCorePackages }:

let
  referencedProject = import ./. { };
in buildDotnetModule rec {
  pname = "ReadingList";
  version = "2.0";
  src = ./.;
  projectFile = "./ReadingList.Carter/ReadingList.Carter.csproj";
  nugetDeps = ./deps.nix;
  dotnet-sdk = dotnetCorePackages.sdk_6_0;
  dotnetFlags = [ "--runtime linux-x64" ];

  executables = [ "ReadingList.Carter" ]; 
  
  packNupkg = true; # This packs the project as "foo-0.1.nupkg" at `$out/share`.
  nupkgName = "ReadingList";
}