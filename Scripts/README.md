<<<<<<< HEAD
# City-Installer
=======
# City Installer for Linux

## Ubuntu
>>>>>>> 0d55acaaf734dd19725ff32d2238c9d0516de587

To install City Chain Node on Ubuntu 16.04 - as <code>sudo su root</code> run the following command:

<code> bash <( curl https://raw.githubusercontent.com/CityChainFoundation/city-chain/master/Scripts/install_city.sh ) </code>

<<<<<<< HEAD
=======
## Raspberry Pi

>>>>>>> 0d55acaaf734dd19725ff32d2238c9d0516de587
To install a City Chain Node on a Raspberry Pi running Raspian - as <code>sudo su root</code> run the following:

<code> bash <( curl https://raw.githubusercontent.com/CityChainFoundation/city-chain/master/Scripts/install_city_RPI.sh ) </code>

<<<<<<< HEAD
To install a City Chain Node on CentOS - as <code>sudo su root</code> run the following:

<code> bash <( curl https://raw.githubusercontent.com/CityChainFoundation/city-chain/master/Scripts/install_city_CentOS.sh ) </code>
=======
## CentOS

To install a City Chain Node on CentOS (Core) - as <code>sudo su root</code> run the following:

<code>
sudo yum install curl

bash <( curl https://raw.githubusercontent.com/CityChainFoundation/city-chain/master/Scripts/install_city_CentOS.sh )
</code>

Alternative short-URL can be used, this required the -L flag for curl to make it follow redirect:

<code>
bash <( curl -L https://bit.ly/citychain-install-centos )
</code>

## Notes
>>>>>>> 0d55acaaf734dd19725ff32d2238c9d0516de587

If you get the error "bash: curl: command not found", run this first: <code>apt-get -y install curl</code>

thecrypt0hunter(2018) - tips: CaqCxWsdGxbmX9e26yDL9k9PwhDrntbBbP
