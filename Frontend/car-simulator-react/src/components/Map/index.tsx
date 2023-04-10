import { Box } from "@mui/material";
import { Loader } from "google-maps";
import React, { useCallback } from "react";
import { getCurrentPosition } from "utils/geolocation";
import { Route } from "types/models";
import {
  makeLocationPinIcon,
  Map as MapComponent,
  makeLocationCarIcon,
} from "utils/map";
import * as signalR from "@microsoft/signalr";

const googleMapsLoader = new Loader(process.env.REACT_APP_GOOGLE_API_KEY);

const wsApiUrl = process.env.REACT_APP_API_WEBSOCKET as string;

interface IProps {
  races: Route[];
  onFinishRace(id: string): void;
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

export default function Map({ races, onFinishRace }: IProps) {
  const mapRef = React.useRef<MapComponent>();
  const containerMapRef = React.useRef<HTMLElement>();
  const sockeIORef = React.useRef<signalR.HubConnection>();

  const removeMark = useCallback(
    (currentRaces: string[]) => {
      var deletedRace = currentRaces.find(
        (r) => !races.some((cr) => cr.id === r)
      );

      mapRef.current?.removeRoute(deletedRace as string);
    },
    [races]
  );

  const addMark = useCallback(
    async (currentRaces: string[]) => {
      var newRace = races.find((r) => !currentRaces.some((cr) => cr === r.id));

      var color = getColorFromUUID(newRace?.id as string, currentRaces);

      mapRef.current?.addRoute(newRace?.id as string, {
        endMarkerOptions: {
          position: {
            lat: newRace?.endPosition.latitude as number,
            lng: newRace?.endPosition.longitude as number,
          },
          icon: makeLocationPinIcon(color),
        },
        startMarkerOptions: {
          position: {
            lat: newRace?.startPosition.latitude as number,
            lng: newRace?.startPosition.longitude as number,
          },
          icon: makeLocationPinIcon(color),
        },
        currentMarkerOptions: {
          position: {
            lat: newRace?.startPosition?.latitude as number,
            lng: newRace?.startPosition.longitude as number,
          },
          icon: makeLocationPinIcon("#000"),
        },
      });

      if (sockeIORef.current?.connectionId) {
        try {
          await sockeIORef.current.send(
            "SendMessage",
            sockeIORef.current.connectionId,
            newRace?.id
          );
        } catch (e) {
          console.log(e);
        }
      }
    },
    [races]
  );

  ///handle pins
  React.useEffect(() => {
    var currentRaces = Object.keys(mapRef.current?.routes || []);

    if (races.length > currentRaces.length) {
      addMark(currentRaces);
    } else if (races.length < currentRaces.length) {
      removeMark(currentRaces);
    }
  }, [races, addMark, removeMark]);

  //render maps
  React.useEffect(() => {
    (async () => {
      const [, position] = await Promise.all([
        googleMapsLoader.load(),
        getCurrentPosition({ enableHighAccuracy: true }),
      ]);
      mapRef.current = new MapComponent(containerMapRef.current as Element, {
        zoom: 15,
        center: position,
      });
    })();
  }, []);

  React.useEffect(() => {
    sockeIORef.current = new signalR.HubConnectionBuilder()
      .withUrl(wsApiUrl)
      .withAutomaticReconnect()
      .build();
  }, []);

  React.useEffect(() => {
    if (!sockeIORef.current?.connectionId) {
      sockeIORef.current
        ?.start()
        .then(() => {
          console.log("connected");
        })
        .catch((err) => {
          console.log("error signal R");
        });
    }

    const handler = (message: any) => {
      console.log(message);

      // currentMarkerOptions: {
      //   position: {
      //     lat: newRace?.startPosition?.latitude as number,
      //     lng: newRace?.startPosition.longitude as number,
      //   },
      //   icon: makeLocationPinIcon("#000"),
      // },

      mapRef.current?.moveCurrentMarker(
        message.orderId,

        {
          lat: Number(message.currentRoute.latitude),
          lng: Number(message.currentRoute.longitude),
        }
      );

      if (message.isFinished) {
        onFinishRace(message.orderId);
      }

      // updateRaces({
      //   latitude: Number(message.currentRoute.latitude),
      //   longitude: Number(message.currentRoute.longitude),
      //   id: message.orderId,
      // });
    };

    sockeIORef.current?.on("UpdateRoute", handler);

    return () => {
      sockeIORef.current?.off("UpdateRoute", handler);
    };
  }, [sockeIORef, races]);

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
