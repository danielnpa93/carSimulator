export interface Route {
  title: string;
  id: string;
  startPosition: Position;
  endPosition: Position;
  currentPosition: Position;
}

type Position = {
  latitude: number;
  longitude: number;
};
