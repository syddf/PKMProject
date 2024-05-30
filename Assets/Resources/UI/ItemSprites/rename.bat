@echo off
setlocal enabledelayedexpansion

rem 循环处理当前目录下的所有.png文件
for %%f in (*.png) do (
    rem 获取文件名（不包括扩展名）
    set "filename=%%~nf"
    
    rem 移除前缀Dream_
    set "filename=!filename:Dream_=!"
    
    rem 移除后缀_Sprite
    set "filename=!filename:_Sprite=!"

    rem 重命名文件
    ren "%%f" "!filename!.png"
)

echo 所有文件处理完成！
pause