SET version=1.3.6
SET targetDir="Releases\TerraMap-%version%\"
mkdir %targetDir%
copy "TerraMap\bin\Debug\CommandLine.dll" %targetDir%
copy "TerraMap\bin\Debug\Microsoft.Threading.Tasks.dll" %targetDir%
copy "TerraMap\bin\Debug\sets-default.xml" %targetDir%
copy "TerraMap\bin\Debug\System.Threading.Tasks.dll" %targetDir%
copy "TerraMap\bin\Debug\TerraMap.Data.dll" %targetDir%
copy "TerraMap\bin\Debug\TerraMap.Data.pdb" %targetDir%
copy "TerraMap\bin\Debug\TerraMap.exe" %targetDir%
copy "TerraMap\bin\Debug\TerraMap.exe.config" %targetDir%
copy "TerraMap\bin\Debug\TerraMap.pdb" %targetDir%
copy "TerraMap\bin\Debug\TerraMapCmd.exe" %targetDir%
copy "TerraMap\bin\Debug\TerraMapCmd.pdb" %targetDir%
copy "TerraMap\bin\Debug\tiles.xml" %targetDir%
copy "TerraMap\bin\Debug\WPFExtensions.dll" %targetDir%
copy "TerraMap\bin\Debug\WriteableBitmapEx.Wpf.dll" %targetDir%
copy "TerraMap\bin\Debug\Xceed.Wpf.Toolkit.dll" %targetDir%
cd Releases
"C:\Program Files\7-Zip\7z.exe" a "TerraMap-%version%.zip" "TerraMap-%version%\"
cd ..