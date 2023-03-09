import { Box } from "@mui/material";
import { Loader } from "google-maps";
import React from "react";
import { Route } from "types/models";
import { getCurrentPosition } from "utils/geolocation";
import { makeLocationPinIcon, Map as MapComponent } from "utils/map";

const googleMapsLoader = new Loader(process.env.REACT_APP_GOOGLE_API_KEY);

interface IProps {
  races: Route[];
}

const colors = [
  "#FF4136",
  "#2ECC40",
  "#0074D9",
  "#FFDC00",
  "#B10DC9",
  "#FF851B",
  "#85144b",
  "#3D9970",
  "#001f3f",
  "#7FDBFF",
];

const getColorFromUUID = (uuid: string, storedUUIDs: string[]) => {
  // Check if the UUID is already stored
  const index = storedUUIDs.indexOf(uuid);
  let colorIndex;

  if (index !== -1) {
    // If the UUID is already stored, use its color index
    colorIndex = index;
  } else {
    // If the UUID is not stored, pick a color index based on the number of stored UUIDs
    const numStoredUUIDs = storedUUIDs.length;
    colorIndex = numStoredUUIDs % colors.length;

    // Check if the selected color has already been used
    if (
      numStoredUUIDs >= colors.length &&
      storedUUIDs.indexOf(storedUUIDs[colorIndex]) !== -1
    ) {
      // If the selected color has already been used, find the next available color
      for (let i = 0; i < colors.length; i++) {
        const nextColorIndex = (colorIndex + i) % colors.length;
        if (storedUUIDs.indexOf(storedUUIDs[nextColorIndex]) === -1) {
          colorIndex = nextColorIndex;
          break;
        }
      }
    }
  }

  return colors[colorIndex];
};

export default function Map({ races }: IProps) {
  const mapRef = React.useRef<MapComponent>();
  const containerMapRef = React.useRef<HTMLElement>();
  //const [currentRaces, setCurrentRaces] = React.useState<Route[]>([]);

  const removeMark = (currentRaces: string[]) => {
    var deletedRace = currentRaces.find(
      (r) => !races.some((cr) => cr.id === r)
    );

    mapRef.current?.removeRoute(deletedRace as string);
  };

  const addMark = (currentRaces: string[]) => {
    var newRace = races.find((r) => !currentRaces.some((cr) => cr === r.id));

    var color = getColorFromUUID(newRace?.id as string, currentRaces);

    mapRef.current?.addRoute(newRace?.id as string, {
      endMarkerOptions: {
        position: {
          lat: newRace?.startPosition.latitude as number,
          lng: newRace?.startPosition.longitude as number,
        },
        icon: makeLocationPinIcon(color),
      },
      startMarkerOptions: {
        position: {
          lat: newRace?.endPosition.latitude as number,
          lng: newRace?.endPosition.longitude as number,
        },
        icon: makeLocationPinIcon(color),
      },
    });
  };

  React.useEffect(() => {
    var currentRaces = Object.keys(mapRef.current?.routes || []);

    if (races.length > currentRaces.length) {
      addMark(currentRaces);
    } else if (races.length < currentRaces.length) {
      removeMark(currentRaces);
    }
  }, [races]);

  React.useEffect(() => {
    (async () => {
      const [, position] = await Promise.all([
        googleMapsLoader.load(),
        getCurrentPosition({ enableHighAccuracy: true }),
      ]);

      //const divMap = document.getElementById("map_container") as HTMLElement;

      mapRef.current = new MapComponent(containerMapRef.current as Element, {
        zoom: 15,
        center: position,
      });

      //   mapRef.current = new google.maps.Map(
      //     containerMapRef.current as HTMLElement,
      //     {
      //       zoom: 15,
      //       center: position,
      //     }
      //   );
    })();
  }, []);

  return (
    <Box
      ref={containerMapRef}
      sx={{
        height: "100%",
        background: "#fff",
      }}
    ></Box>
  );
}
