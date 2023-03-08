print(
  "Alteracao executadas 0 ........................................................."
);
console.log("EXCUCAO FEITA 0");

db.routes.insertMany([
  {
    _id: UUID()
      .toString("hex")
      .match(/^(.{8})(.{4})(.{4})(.{4})(.{12})$/)
      .slice(1, 6)
      .join("-"),
    title: "Primeiro",
    startPosition: { latitude: -15.8294, longitude: -47.92923 },
    endPosition: { latitude: -15.82942, longitude: -47.92765 },
  },
  {
    _id: UUID()
      .toString("hex")
      .match(/^(.{8})(.{4})(.{4})(.{4})(.{12})$/)
      .slice(1, 6)
      .join("-"),
    title: "Segundo",
    startPosition: { latitude: -15.82449, longitude: -47.92756 },
    endPosition: { latitude: -15.8276, longitude: -47.92621 },
  },
  {
    _id: UUID()
      .toString("hex")
      .match(/^(.{8})(.{4})(.{4})(.{4})(.{12})$/)
      .slice(1, 6)
      .join("-"),
    title: "Terceiro",
    startPosition: { latitude: -15.82331, longitude: -47.92588 },
    endPosition: { latitude: -15.82758, longitude: -47.92532 },
  },
]);

print(
  "Alteracao executadas ........................................................."
);
console.log("EXCUCAO FEITA");
