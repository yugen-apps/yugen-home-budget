dotnet_sdk="/snap/dotnet-sdk/current"
dotnet_tools=~/.dotnet/tools
echo "PATH="${PATH}:${dotnet_sdk}:${dotnet_tools}"" | sudo tee /etc/environment

echo "Defaults secure_path="${PATH}:${dotnet_sdk}"" | sudo tee -a /etc/sudoers

echo "DOTNET_ROOT="${dotnet_sdk}"" | sudo tee -a /etc/environment

echo $PATH
echo $DOTNET_ROOT

sudo cat /etc/environment
sudo cat /etc/sudoers

apt list *dotnet* --installed

curl --proto '=https' -sSf https://dnvm.net/install.sh | sh

Found /home/pandsharp/.profile
Found /home/pandsharp/.bashrc

git config --global user.name "Emiliano Magliocca"
git config --global user.email "emilianom@kortext.com"
