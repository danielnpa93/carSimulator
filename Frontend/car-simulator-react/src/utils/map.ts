export class Route {
  public startMarker: google.maps.Marker;
  public endMarker: google.maps.Marker;
  public currentMarker: google.maps.Marker;
  public directionRenderer: google.maps.DirectionsRenderer;

  constructor(options: {
    startMarkerOptions: google.maps.ReadonlyMarkerOptions;
    endMarkerOptions: google.maps.ReadonlyMarkerOptions;
    currentMarkerOptions: google.maps.ReadonlyMarkerOptions;
  }) {
    const { startMarkerOptions, endMarkerOptions, currentMarkerOptions } =
      options;
    this.startMarker = new google.maps.Marker(startMarkerOptions);
    this.endMarker = new google.maps.Marker(endMarkerOptions);
    this.currentMarker = new google.maps.Marker(currentMarkerOptions);

    const strokeColor = (
      this.startMarker.getIcon() as google.maps.ReadonlySymbol
    ).strokeColor;

    this.directionRenderer = new google.maps.DirectionsRenderer({
      suppressMarkers: true,
      polylineOptions: {
        strokeColor,
        strokeOpacity: 0.5,
        strokeWeight: 5,
      },
    });

    this.directionRenderer.setMap(this.startMarker.getMap() as google.maps.Map);

    this.calculateRoute();
  }

  private calculateRoute() {
    const startPosition = this.startMarker.getPosition() as google.maps.LatLng;
    const endPosition = this.endMarker.getPosition() as google.maps.LatLng;

    new google.maps.DirectionsService().route(
      {
        origin: startPosition,
        destination: endPosition,
        travelMode: google.maps.TravelMode.DRIVING,
      },
      (result, status) => {
        if (status === "OK") {
          //console.log(result);

          this.directionRenderer.setDirections(result);

          return;
        }

        console.log("error");
        throw new Error(status);
      }
    );
  }
}

export class Map {
  public map: google.maps.Map;
  public routes: { [id: string]: Route } = {};
  constructor(element: Element, options: google.maps.MapOptions) {
    this.map = new google.maps.Map(element, options);
  }

  moveCurrentMarker(id: string, position: google.maps.LatLngLiteral) {
    this.routes[id].currentMarker.setPosition(position);
  }

  addRoute(
    id: string,
    routeOptions: {
      startMarkerOptions: google.maps.ReadonlyMarkerOptions;
      endMarkerOptions: google.maps.ReadonlyMarkerOptions;
      currentMarkerOptions: google.maps.ReadonlyMarkerOptions;
    }
  ) {
    const { endMarkerOptions, startMarkerOptions, currentMarkerOptions } =
      routeOptions;
    this.routes[id] = new Route({
      startMarkerOptions: { ...startMarkerOptions, map: this.map },
      endMarkerOptions: { ...endMarkerOptions, map: this.map },
      currentMarkerOptions: { ...currentMarkerOptions, map: this.map },
    });

    this.fitBounds();
  }

  removeRoute(id: string) {
    this.routes[id].startMarker.setMap(null);
    this.routes[id].endMarker.setMap(null);
    this.routes[id].currentMarker.setMap(null);
    this.routes[id].directionRenderer.setMap(null);

    delete this.routes[id];
  }

  private fitBounds() {
    const bounds = new google.maps.LatLngBounds();

    Object.keys(this.routes).forEach((id: string) => {
      const route = this.routes[id];
      bounds.extend(route.startMarker.getPosition() as any);
      bounds.extend(route.endMarker.getPosition() as any);
    });

    this.map.fitBounds(bounds);
  }
}

export const makeLocationPinIcon = (color?: string) => {
  return {
    path: "M10,0 C4.5,0 0,4.5 0,10 C0,18 10,30 10,30 C10,30 20,18 20,10 C20,4.5 15.5,0 10,0 Z M10,14 A4,4 0 1 1 10,22 A4,4 0 0 1 10,14 Z",
    fillColor: color || "#000",
    strokeColor: color || "#000",
    strokeWeight: 1,
    fillOpacity: 1,
    anchor: new google.maps.Point(10, 30),
  };
};

export const makeLocationCarIcon = (color?: string) => {
  return {
    path: "M50 55 A5 5 0 1 0 50 45 A5 5 0 1 0 50 55 Z",
    fillColor: "none",
    strokeColor: color || "#000",
    strokeWeight: 1,
    fillOpacity: 1,
    anchor: new google.maps.Point(0, 0),
  };
};
