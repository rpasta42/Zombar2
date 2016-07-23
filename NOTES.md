- [ ] World divided into 100m x 100m squares (or smaller area)
- [ ] log-in server picks spawn square
- [ ] write in UnityNetworkManager/Python/Rust/Node/SchemeScript. Decide UDP/TCP/WebSocket, etc, etc.
- [ ] Each square range is a temporary "area session worker"
	- [ ] each "area session worker" is stored on a server
	- [ ] each server has many "area session workers". Try to divide them evenly (serverAverage = 0; foreach (x in AreaSessionWorker) serverAverage += x.numPlayers)
	- [ ] try to store "area session workers" based on physical amount of intersections (squares that are touching each other)
	- [ ] also keep track of people moving between areas, and where the number is high, keep squares on same server (if lots of back-forth traffic, store on same physical server)
- [ ] square seams
	- [ ] neighboring squares share information for the visible range (both servers sync status & position of players on each other's border)
- [ ] Each Player NetworkSync rotation of players on same square (if distance is visible)
	- [ ] Have ability to disable sharing your location
- [ ] Zombies synced based on GPS, and compass
- [ ] Build/scavange during day, defend during night
	- [ ] Possibly have day/night cycles short (like 10 min-ish)
- [ ] possibly build fort areas around houses
	- [ ] sleep area safe by default (when you go offline or something)
	- [ ] have main area like house or room, lay out your own windows, doors, sniper nests.
		- Can't shoot zombie in a fort unless you're in sniper nest or something
	- [ ] stash supplies
- [ ] make hunting parties
- [ ] decide what happens if you die

V2 PvP
- [ ] party-only or world mode (for people sketching about getting robbed based on GPS)
- [ ] PvP mode, snipers, hiding, camouflage
- [ ] compass based, rotation

V3 Framework
- [ ] Unity/Android/etc framework for doing anything sensor oriented that combines with graphics/maps/massiveMultiplayer/statisticalData/etc.


API
===
- [ ] If square doesn't have any human population (aka players), do not create one. If we have empty regions, a lot of empty AreaSessionWorkers will destroy the server.
	- At first (initial version of game), only create Squares around people, and somehow try to fit multiple in same square
		- at first just have 1 server
- [ ] Encryption/Streaming/Network Stack/DNS Load-balancer
   - [ ] [http://gamedev.stackexchange.com/questions/115615/should-i-encrypt-my-multiplayer-network-traffic](link)
   - [ ] for performance reasons, might want to Encrypt CheckSym of correct data with Server/Client Public Keys, and send data unecrypted
   - [ ] might want to encrypt position, but not rotation
   - [ ] if each square is big, do not load data for all players on that square. Only load portions in visible range
   - [ ] If have linodes or AWSes in different physical locations, factor in distribution of AreaSessionWorkers
   - [ ] (https://www.nginx.com/resources/glossary/load-balancing/)[nginx load balancing)
      - [ ] "The best load balancers can handle session persistence as needed. Another use case for session persistence is when an upstream server stores information requested by a user in its cache to boost performance. Switching servers would cause that information to be fetched for the second time, creating performance inefficiencies."

class AreaSessionWorker
	Vector<AreaSessionWorker> getNeighbors(); //all squares around us
	Vector<Player> getPlayers();
	Vector<Player> getBorderPlayers(); //players on intersection of 2 AreaSessionWorkers
	
