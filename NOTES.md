- [ ] World divided into 100m x 100m squares (or smaller area)
- [ ] log-in server picks spawn square
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
- [ ] PvP mode, snipers, hiding, camouflage 
- [ ] compass based, rotation
